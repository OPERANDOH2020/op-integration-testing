import requests
from xml.etree import ElementTree
import test_helpers.utils
import Settings

class InvalidServiceTicketError(Exception):
    """The service ticket is not valid to access the requested service"""

class InvalidServiceIdError(Exception):
    """The service Id is not known"""

class UnknownServiceTicketError(Exception):
    """An unknown error was found when validating a service ticket"""

class AuthenticationApiService(object):
    
    root_url = Settings.aapi_url;
    SingleSignOnOperando_url = root_url + "/aapi/tickets"
    ValidateServiceTicket_url = root_url + "/aapi/tickets/{st}/validate"
    GetServiceTicket_url = root_url + "/aapi/tickets/{tgt}"

    def __init__(self, timeout):
        self.TIMEOUT = timeout

    def request_TicketGrantingTicket(self, username, password):
        """Request a TGT for the username and password"""
        url = self.SingleSignOnOperando_url;
        data = { "username": username, "password": password }

        response = requests.post(url, json=data, timeout=self.TIMEOUT)
        
        test_helpers.utils.raise_on_error(response)

        return response

    def request_ServiceTicket(self, tgt, service_id):
        """Request a ST for the service_id using the tgt"""
        url = self.GetServiceTicket_url.format(tgt=tgt);

        response = requests.post(url, data=service_id, timeout=self.TIMEOUT)
        
        test_helpers.utils.raise_on_error(response)

        return response

    def validate_ServiceTicket(self, st, service_id):
        """Validate the ST for the service_id"""
        url = self.ValidateServiceTicket_url.format(st=st);

        params = { "serviceId": service_id }

        response = requests.get(url, params=params, timeout=self.TIMEOUT)

        content = ElementTree.fromstring(response.text)

        # confirm that the response is correct
        ns = {"cas" : "http://www.yale.edu/tp/cas"}
        failure = content.find("cas:authenticationFailure", ns) is not None
        success = content.find("cas:authenticationSuccess", ns) is not None
        
        if success and response.status_code == 200:
            return response # successful operation
        elif failure and response.status_code == 400:
            raise InvalidServiceTicketError(response.text)
        elif response.status_code == 500:
            raise InvalidServiceIdError(response.text)
        else:
            raise UnknownServiceTicketError(response.text)