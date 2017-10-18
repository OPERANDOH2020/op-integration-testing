import unittest
import test_helpers.utils
from test_helpers.aapi_service import AuthenticationApiService, InvalidServiceIdError, InvalidServiceTicketError, UnknownServiceTicketError


class AuthenticationApiTests(unittest.TestCase):
   
    
    valid_service_ids = [
        "/gatekeeper",
        "GET/osp/bda/jobs/.*/reports",
        "GET/osp/reports/.*",
        "GET/regulator/reports/.*",
        "POST/regulator/regulations",
        "PUT/regulator/regulations/.+",
        "op-pdr/dan",
        "/gatekeeper",
        "op-pdr/rpm/built-in",
        "/operando/rm/",
        "/operando/webui/reports/"
        ]

    TIMEOUT = 5

    aapi_service = AuthenticationApiService(TIMEOUT)

    
    def test_FullWorkflow(self):
        """This tests the following workflow:
        * A user requests a TGT using valid credentials
        * A user uses this TGT to request a ST for a single service
        * An internal module can then check this ST is valid
        """

        username = "Dani" # valid
        password = "Dani" # valid

        # get TGT
        response = self.aapi_service.request_TicketGrantingTicket(username, password)
        tgt = response.text
        self.assertIsNotNone(tgt)
        self.assertIsNot("", tgt)

        for service_id in self.valid_service_ids:
            print "Testing service_id '{}'".format(service_id)
            
            # get ST
            response = self.aapi_service.request_ServiceTicket(tgt, service_id)
            st = response.text
            self.assertIsNotNone(st)
            self.assertIsNot("", st)

            # Validate can access service with ST
            response = self.aapi_service.validate_ServiceTicket(st, service_id)
            print response.text

    def testSingleSignOnOperando_ReturnsDifferentTGTs(self):
        """We expect different TGTs to be generated"""

        service_id = self.valid_service_ids[0]
        username = "Dani" # valid
        password = "Dani" # valid

        # get TGT
        response = self.aapi_service.request_TicketGrantingTicket(username, password)
        first_tgt = response.text

        response = self.aapi_service.request_TicketGrantingTicket(username, password)
        second_tgt = response.text

        self.assertNotEqual(first_tgt, second_tgt)
        

    def testSingleSignOnOperando_TicketGrantingTicket_NotReturned_If_InvalidCredentials(self):
        service_id = self.valid_service_ids[0]
        username = "INVALID!"
        password = "VERY INVALID!"

        # get TGT
        with self.assertRaises(test_helpers.utils.ClientError):
            response = self.aapi_service.request_TicketGrantingTicket(username, password)
    

    def testGetServiceTicket_ServiceTicket_NotReturned_If_InvalidTicketGrantingTicketUsed(self):
        service_id = self.valid_service_ids[0]
        tgt = "INVALID!!!" # do not get the TGT -- make up an invalid one

        # get ST
        with self.assertRaises(test_helpers.utils.ServerError):
            response = self.aapi_service.request_ServiceTicket(tgt, service_id)

            
    def testGetServiceTicket_ServiceTicket_NotReturned_If_InvalidServiceId(self):
        service_id = "INVALID!"
        username = "Dani" # valid
        password = "Dani" # valid

        # get TGT
        response = self.aapi_service.request_TicketGrantingTicket(username, password)
        tgt = response.text
        self.assertIsNotNone(tgt)
        self.assertIsNot("", tgt)

        # get ST
        with self.assertRaises(test_helpers.utils.ClientError):
            response = self.aapi_service.request_ServiceTicket(tgt, service_id)

    def testGetServiceTicket_Invalid_If_InvalidServiceId(self):
        service_id = "INVALID!"
        username = "Dani" # valid
        password = "Dani" # valid

        # get TGT
        response = self.aapi_service.request_TicketGrantingTicket(username, password)
        tgt = response.text
        self.assertIsNotNone(tgt)
        self.assertIsNot("", tgt)

        # get ST
        with self.assertRaises(test_helpers.utils.ClientError):
            response = self.aapi_service.request_ServiceTicket(tgt, service_id)

    def testValidateServiceTicket_Invalid_If_InvalidServiceTicket(self):
        service_id = self.valid_service_ids[0]
        st = "INVALID!!!"

        # validate can access with ST
        with self.assertRaises(InvalidServiceTicketError):
            response = self.aapi_service.validate_ServiceTicket(st, service_id)

    def testValidateServiceTicket_Invalid_If_InvalidServiceId(self):
        valid_service_id = self.valid_service_ids[0]
        invalid_service_id = "INVALID!!!"
        username = "Dani"
        password = "Dani"

        # get TGT
        response = self.aapi_service.request_TicketGrantingTicket(username, password)
        tgt = response.text
        self.assertIsNotNone(tgt)
        self.assertIsNot("", tgt)

        # get ST
        response = self.aapi_service.request_ServiceTicket(tgt, valid_service_id)
        st = response.text
        self.assertIsNotNone(st)
        self.assertIsNot("", st)

        # validate cannot access with invalid service id
        with self.assertRaises(InvalidServiceIdError):
            response = self.aapi_service.validate_ServiceTicket(st, invalid_service_id)
                
            
if __name__ == '__main__':
    t = unittest.main(exit=False)
    if not (t.result.errors or t.result.failures):
        print "ALL SUCCESSFUL"
    else:
        print "ERROR"