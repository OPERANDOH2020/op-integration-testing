import requests
from test_helpers.aapi_service import AuthenticationApiService
from test_helpers.AuthenticatableTestCase import AuthenticatableTestCase
import unittest

class BdaTests(AuthenticatableTestCase):

    url = "http://integration.operando.esilab.org:8098/operando/core/bigdata/jobs/{job_id}/reports/"
    
    service_id = "GET/osp/bda/jobs/.*/reports"

    valid_user_id="1"

    valid_job_id="1"

    def _get_bda_report(self, service_ticket, job_id, user_id):
        url_with_params = self.url.format(job_id=job_id)
        headers = {
            "service-ticket": service_ticket,
            "psp-user": user_id
        }
        response = requests.get(url_with_params, headers=headers, timeout=self.TIMEOUT)
        print response.text
        return response

    def _get_service_ticket(self):
        return super(BdaTests, self)._get_service_ticket_for(self.service_id)

    def test_get_invalid_job_id_returns_404(self):
        st = self._get_service_ticket()

        response = self._get_bda_report(st, "invalid_job_id", self.valid_user_id)

        self.assertEquals(404, response.status_code)

    def test_get_invalid_user_id_returns_404(self):
        st = self._get_service_ticket()

        response = self._get_bda_report(st, self.valid_job_id, "invalid_user_id")

        self.assertEquals(404, response.status_code)

    def test_get_successful_get_returns_200(self):
        st = self._get_service_ticket()

        response = self._get_bda_report(st, self.valid_job_id, self.valid_user_id)

        self.assertEquals(200, response.status_code)

    def test_get_invalid_service_ticket_returns_401(self):
        st = "invalid-service-ticket"

        response = self._get_bda_report(st, self.valid_job_id, self.valid_user_id)

        self.assertEquals(401, response.status_code)

if __name__ == '__main__':
    t = unittest.main(exit=False)
    if not (t.result.errors or t.result.failures):
        print "ALL SUCCESSFUL"
    else:
        print "ERROR"