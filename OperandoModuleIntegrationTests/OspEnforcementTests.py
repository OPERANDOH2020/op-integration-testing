import requests
from test_helpers.utils import ClientError, raise_on_error
from test_helpers.aapi_service import AuthenticationApiService
from test_helpers.AuthenticatableTestCase import AuthenticatableTestCase
import json
from xml.etree import ElementTree

class OspEnforcementTests(AuthenticatableTestCase):
   
    post_url = "http://ose.integration.operando.lan.esilab.org:8094/operando/core/ose/"
    service_id = "ose/regulations/.*"

    def _post_regulations(self, data, service_ticket):
        url = self.post_url + "/regulations"
        headers = {"service-ticket": service_ticket}
        response = requests.post(url, json=data, headers=headers, timeout=self.TIMEOUT)
        print response.text
        return response

    def _create_RegulationsPost_model(self, 
                                      legislation_sector="Default test sector", reg_id=None, reason=None,
                                      private_information_type=None, action=None, required_consent=None):
        model = {
            "legislation_sector": legislation_sector
        }
        if reg_id is not None: 
            model["reg_id"] = reg_id
        if private_information_type is not None: 
            model["private_information_type"] = private_information_type
        if action is not None: 
            model["action"] = action
        if required_consent is not None: 
            model["required_consent"] = action
        
        return { "regulation": model }
   
    def _get_service_ticket(self):
        return super(OspEnforcementTests, self)._get_service_ticket_for(self.service_id)

    def test_invalid_service_ticket_does_not_grant_access(self):
        invalid_st = "invalid service ticket!"
        
        model = self._create_RegulationsPost_model()
        response = self._post_regulations(model, invalid_st)
        
        with self.assertRaises(ClientError):
            raise_on_error(response)
        
    def test_successfull_post_returns_201(self):
        st = self._get_service_ticket()

        model = self._create_RegulationsPost_model()
        response = self._post_regulations(model, st)

        self.assertEqual(201, response.status_code)
        
    
    def test_invalid_data_is_client_error(self):
        """legislation_sector is marked as required. We should get a client error if it is not provided"""
        st = self._get_service_ticket()
        
        model = {"invalid": "model"}
        response = self._post_regulations(model, st)

        with self.assertRaises(ClientError):
            raise_on_error(response)
    
            
if __name__ == '__main__':
    unittest.main()