import requests
from test_helpers.aapi_service import AuthenticationApiService
from test_helpers.AuthenticatableTestCase import AuthenticatableTestCase
import unittest

class RApiTests(AuthenticatableTestCase):

    url = "http://integration.operando.esilab.org:8133/operando/interfaces/rapi/regulator/osps/{osp_id}/compliance-report"
    
    service_id = "GET/osps/{osp-id}/compliance-report"

    valid_osp_id = "1"

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

        response = self._get_compliance_report(st, self.valid_osp_id)

        self.assertEquals(200, response.status_code)

    def test_get_invalid_service_ticket_returns_401(self):
        st = "invalid-service-ticket"

        response = self._get_compliance_report(st, self.valid_osp_id)

        self.assertEquals(401, response.status_code)

if __name__ == '__main__':
    t = unittest.main(exit=False)
    if not (t.result.errors or t.result.failures):
        print "ALL SUCCESSFUL"
    else:
        print "ERROR"