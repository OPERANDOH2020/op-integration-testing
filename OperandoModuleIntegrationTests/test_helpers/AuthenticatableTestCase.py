import unittest
from test_helpers.aapi_service import AuthenticationApiService

class AuthenticatableTestCase(unittest.TestCase):
    """Base class for tests that require ticket generating tickets and service tickets
    
    It automatically gets a tgt for each test (using the same tgt for all tests in the derived class)

    It is also able to get a valid service ticket for the specified service_id    
    """

    TIMEOUT = 5 # seconds
    
    _aapi_service = AuthenticationApiService(TIMEOUT)

    @classmethod
    def setUpClass(cls):
        """Creates a valid TicketGeneratingTicket for all tests in this class"""

        # valid user credentials
        username = "Dani"
        password = "Dani"

        response = cls._aapi_service.request_TicketGrantingTicket(username, password)
        cls.tgt = response.text

    
    def _get_service_ticket_for(self, service_id):
        """Get a service ticket for the specified service_id"""
        response = self._aapi_service.request_ServiceTicket(self.tgt, service_id)
        st = response.text
        self.assertIsNotNone(st)
        self.assertIsNot("", st)
        return st