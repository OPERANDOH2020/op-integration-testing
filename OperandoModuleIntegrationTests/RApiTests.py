import requests
from test_helpers.aapi_service import AuthenticationApiService
from test_helpers.AuthenticatableTestCase import AuthenticatableTestCase
from test_helpers.pdb_service import PoliciesDatabaseService
import unittest

class RApiTests(AuthenticatableTestCase):

    _pdb_service = PoliciesDatabaseService(AuthenticatableTestCase.TIMEOUT)

    url = "http://integration.operando.esilab.org:8133/operando/interfaces/rapi/regulator/osps/{osp_id}/compliance-report"
    
    service_id = "GET/osps/{osp-id}/compliance-report"

    def _get_compliance_report(self, service_ticket, osp_id):
        url_with_params = self.url.format(osp_id=osp_id)
        headers = {
            "service-ticket": service_ticket,
        }
        response = requests.get(url_with_params, headers=headers, timeout=self.TIMEOUT)
        print response.text
        return response

    def _get_service_ticket(self):
        return super(RApiTests, self)._get_service_ticket_for(self.service_id)

    def test_get_invalid_osp_id_returns_404(self):
        st = self._get_service_ticket()

        response = self._get_compliance_report(st, "invalid_osp_id", )

        self.assertEquals(404, response.status_code)

    def test_get_successful_get_returns_200(self):
        st = self._get_service_ticket()

        valid_osp_id = self._pdb_service.get_valid_osp_id()

        response = self._get_compliance_report(st, valid_osp_id)

        self.assertEquals(200, response.status_code)

    def test_get_invalid_service_ticket_returns_401(self):
        st = "invalid-service-ticket"

        valid_osp_id = self._pdb_service.get_valid_osp_id()

        response = self._get_compliance_report(st, valid_osp_id)

        self.assertEquals(401, response.status_code)

if __name__ == '__main__':
    t = unittest.main(exit=False)
    if not (t.result.errors or t.result.failures):
        print "ALL SUCCESSFUL"
    else:
        print "ERROR"