import requests
from test_helpers.utils import ClientError, raise_on_error
from test_helpers.aapi_service import AuthenticationApiService
from test_helpers.pdb_service import PoliciesDatabaseService
from test_helpers.AuthenticatableTestCase import AuthenticatableTestCase
import json
import unittest
import Settings

class PolicyComputationTests(AuthenticatableTestCase):
   
    post_url = Settings.pc_url + "/regulations/{reg_id}"

    _pdb_service = PoliciesDatabaseService(AuthenticatableTestCase.TIMEOUT)
    invalid_reg_id = -1 # this should always be invalid

    def _get_valid_regulation_id(self):
        service_ticket = self._get_service_ticket_for(self._pdb_service.regulations_service_id)
        model = self._pdb_service.create_RegulationsPost_model()
        response = self._pdb_service.create_regulation(model, service_ticket)
        raise_on_error(response)

        return self._pdb_service.get_reg_id_from_response(response)
    
    def _post_regulation(self, reg_id, service_ticket):
        url = self.post_url.format(reg_id=reg_id)
        headers = {"service-ticket": service_ticket}
        response = requests.post(url, headers=headers, timeout=self.TIMEOUT)
        print response.text
        return response
   
    def _get_service_ticket(self):
        return super(PolicyComputationTests, self)._get_service_ticket_for(self._pdb_service.regulations_service_id)

    def test_post_invalid_service_ticket_does_not_grant_access(self):
        invalid_st = "invalid-service-ticket!"
        
        reg_id = self._get_valid_regulation_id()
        response = self._post_regulation(reg_id, invalid_st)
        
        with self.assertRaises(ClientError):
            raise_on_error(response)
        
    def test_post_successful_post_returns_200_status(self):
        st = self._get_service_ticket()

        reg_id = self._get_valid_regulation_id()
        response = self._post_regulation(reg_id, st)

        self.assertEqual(200, response.status_code)
    
    def test_post_invalid_regId_returns_404(self):
        st = self._get_service_ticket()
        
        reg_id = self.invalid_reg_id
        response = self._post_regulation(reg_id, st)
        
        self.assertEqual(404, response.status_code)
            
if __name__ == '__main__':
    t = unittest.main(exit=False)
    if not (t.result.errors or t.result.failures):
        print "ALL SUCCESSFUL"
    else:
        print "ERROR"