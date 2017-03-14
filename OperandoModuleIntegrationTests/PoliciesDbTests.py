import requests
from test_helpers.utils import ClientError, raise_on_error
from test_helpers.aapi_service import AuthenticationApiService
from test_helpers.pdb_service import PoliciesDatabaseService
from test_helpers.AuthenticatableTestCase import AuthenticatableTestCase
import json
from xml.etree import ElementTree
import unittest

class PoliciesDbTests(AuthenticatableTestCase):
   
    _pdb_service = PoliciesDatabaseService(AuthenticatableTestCase.TIMEOUT)
   
    def _get_service_ticket(self, service_id):
        return super(PoliciesDbTests, self)._get_service_ticket_for(service_id)

    # Regulation Input [RAPI->PDB]

    def test_post_regulation_invalid_service_ticket_does_not_grant_access(self):
        invalid_st = "invalid-service-ticket!"
        
        model = self._pdb_service.create_RegulationsPost_model()
        response = self._pdb_service.create_regulation(model, invalid_st)
        
        with self.assertRaises(ClientError):
            raise_on_error(response)
        
    def test_post_regulation_successful_post_returns_201_status_and_regId(self):
        st = self._get_service_ticket(PoliciesDatabaseService.regulations_service_id)

        model = self._pdb_service.create_RegulationsPost_model()
        response = self._pdb_service.create_regulation(model, st)

        self.assertEqual(201, response.status_code)
        
        # confirm that the response contains the newly created reg_id
        
        self.assertIsNotNone(response)
        data = json.loads(response.text)
        self.assertTrue("reg_id" in data)
        reg_id = data["reg_id"]
        self.assertIsNotNone(reg_id)
        
    
    def test_post_regulation_invalid_data_is_client_error(self):
        """legislation_sector is marked as required. We should get a client error if it is not provided"""
        st = self._get_service_ticket(PoliciesDatabaseService.regulations_service_id)
        
        invalid_model = {"invalid": "model"}
        response = self._pdb_service.create_regulation(invalid_model, st)

        with self.assertRaises(ClientError):
            raise_on_error(response)

    # Compliance Report [RAPI->PDB]

    def test_get_privacy_policy_successful_get_returns_200_and_privacy_policy(self):
        st = self._get_service_ticket(PoliciesDatabaseService.osps_service_id)
        valid_osp_id = "1"

        response = self._pdb_service.get_privacy_policy(st, valid_osp_id)

        self.assertEquals(200, response.status_code)

        data = json.loads(response.text)

        self.assertTrue("osp_policy_id" in data)
        osp_policy_id = data["osp_policy_id"]
        self.assertIsNotNone(osp_policy_id)

        self.assertTrue("policies" in data)
        policies = data["policies"]
        self.assertIsNotNone(policies)

    def test_get_privacy_policy_invalid_osp_id_returns_404(self):
        st = self._get_service_ticket(PoliciesDatabaseService.osps_service_id)
        invalid_osp_id = "invalid-osp-id"

        response = self._pdb_service.get_privacy_policy(st, invalid_osp_id)

        self.assertEquals(404, response.status_code)

    def test_get_privacy_policy_invalid_service_ticket_returns_401(self):
        st = "invalid_service_ticket!"
        valid_osp_id = "1"

        response = self._pdb_service.get_privacy_policy(st, valid_osp_id)

        self.assertEquals(401, response.status_code)
    
    
            
if __name__ == '__main__':
    unittest.main()