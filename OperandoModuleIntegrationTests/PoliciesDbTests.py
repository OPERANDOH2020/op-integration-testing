import requests
from test_helpers.utils import ClientError, raise_on_error
from test_helpers.aapi_service import AuthenticationApiService
from test_helpers.pdb_service import PoliciesDatabaseService
from test_helpers.AuthenticatableTestCase import AuthenticatableTestCase
import json
from xml.etree import ElementTree

class PoliciesDbTests(AuthenticatableTestCase):
   
    _pdb_service = PoliciesDatabaseService(AuthenticatableTestCase.TIMEOUT)
   
    def _get_service_ticket(self):
        return super(PoliciesDbTests, self)._get_service_ticket_for(PoliciesDatabaseService.service_id)

    def test_invalid_service_ticket_does_not_grant_access(self):
        invalid_st = "invalid-service-ticket!"
        
        model = self._pdb_service.create_RegulationsPost_model()
        response = self._pdb_service.create_regulation(model, invalid_st)
        
        with self.assertRaises(ClientError):
            raise_on_error(response)
        
    def test_successful_post_returns_201_status_and_regId(self):
        st = self._get_service_ticket()

        model = self._pdb_service.create_RegulationsPost_model()
        response = self._pdb_service.create_regulation(model, st)

        self.assertEqual(201, response.status_code)
        
        # confirm that the response contains the newly created reg_id
        content = ElementTree.fromstring(response.text)
        response_message = content.find("message")
        
        self.assertIsNotNone(response_message)
        data = json.loads(response_message.text)
        self.assertTrue("reg_id" in data)
        reg_id = data["reg_id"]
        self.assertIsNotNone(reg_id)
        
    
    def test_invalid_data_is_client_error(self):
        """legislation_sector is marked as required. We should get a client error if it is not provided"""
        st = self._get_service_ticket()
        
        invalid_model = {"invalid": "model"}
        response = self._pdb_service.create_regulation(invalid_model, st)

        with self.assertRaises(ClientError):
            raise_on_error(response)
    
    
            
if __name__ == '__main__':
    unittest.main()