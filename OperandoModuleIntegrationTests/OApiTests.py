import requests
from test_helpers.AuthenticatableTestCase import AuthenticatableTestCase

class OApiTests(AuthenticatableTestCase):
   
    root_url = "http://integration.operando.esilab.org:8131/operando/interfaces/oapi"
    reportsReportIdGet_url = root_url + "/reports/{report_id}"
    
    service_id = "GET/osp/reports/.*"

    valid_report_id = "1"
    invalid_report_id = "99"
    valid_formats = ["html", "pdf"]

    def _reports_reportId_get(self, report_id, format, service_ticket):
        url = self.reportsReportIdGet_url.format(report_id=report_id)
        headers = {"service-ticket": service_ticket}
        parameters = {"format": format}
        
        response = requests.get(url, headers=headers, params=parameters, timeout=self.TIMEOUT)
        print response.text
        return response

    def _get_service_ticket(self):
        return super(OApiTests, self)._get_service_ticket_for(self.service_id)

    def test_get_invalid_service_ticket_returns_401(self):
        invalid_st = "invalid-service-ticket!"
        format = self.valid_formats[0]
        
        response = self._reports_reportId_get(self.valid_report_id, format, invalid_st)
        
        self.assertEqual(401, response.status_code)
        
    def test_get_invalid_reportId_returns_404(self):
        st = self._get_service_ticket()
        format = self.valid_formats[0]
        
        response = self._reports_reportId_get(self.invalid_report_id, format, st)
        
        self.assertEqual(404, response.status_code)
    
    def test_get_invalid_format_returns_400(self):
        st = self._get_service_ticket()
        invalid_format = "invalid report format!"
        
        response = self._reports_reportId_get(self.valid_report_id, invalid_format, st)
        
        self.assertEqual(400, response.status_code)

    def test_get_successfull_request_returns_200_and_body(self):
        
        for format in self.valid_formats:        
            st = self._get_service_ticket()
            response = self._reports_reportId_get(self.valid_report_id, format, st)
        
            self.assertEqual(200, response.status_code)
            self.assertIsNotNone(response.text)
            self.assertNotEqual("", response.text)
            
if __name__ == '__main__':
    t = unittest.main(exit=False)
    if not (t.result.errors or t.result.failures):
        print "ALL SUCCESSFUL"
    else:
        print "ERROR"