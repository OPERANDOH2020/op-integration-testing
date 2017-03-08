import unittest
import requests
from test_helpers.AuthenticatableTestCase import AuthenticatableTestCase

class ReportGeneratorTests(AuthenticatableTestCase):
   
    url = "http://integration.operando.esilab.org:8122/Report/Report"
    service_id = "/operando/webui/reports/"

    valid_report_id = "4"
    invalid_report_id = "99"
    valid_formats = ["pdf"]

    def _reportsReportIdGet(self, report_id, format, service_ticket):
        headers = {"service-ticket": service_ticket}
        parameters = {"_reportid": report_id, "_format": format}
        
        response = requests.get(self.url, headers=headers, params=parameters, timeout=self.TIMEOUT)
        print response.text
        return response

    def _get_service_ticket(self):
        return super(ReportGeneratorTests, self)._get_service_ticket_for(self.service_id)

    def test_reportsReportIdGet_invalid_service_ticket_returns_401_code(self):
        invalid_st = "invalid-service-ticket!"
        format = self.valid_formats[0]
        
        response = self._reportsReportIdGet(self.valid_report_id, format, invalid_st)
        
        self.assertEqual(401, response.status_code)
        
    def test_reportsReportIdGet_nonexistant_report_id_returns_400(self):
        st = self._get_service_ticket()
        format = self.valid_formats[0]
        
        response = self._reportsReportIdGet(self.invalid_report_id, format, st)
        
        self.assertEqual(400, response.status_code)
    
    def test_reportsReportIdGet_nonexistant_format_returns_400(self):
        st = self._get_service_ticket()
        invalid_format = "invalid report format!"
        
        response = self._reportsReportIdGet(self.valid_report_id, invalid_format, st)
        
        self.assertEqual(400, response.status_code)

    def test_reportsReportIdGet_successful_request_returns_200(self):
        
        for format in self.valid_formats:        
            st = self._get_service_ticket()
            response = self._reportsReportIdGet(self.valid_report_id, format, st)
        
            self.assertEqual(200, response.status_code)
            self.assertIsNotNone(response.text)
            self.assertNotEqual("", response.text)
            
if __name__ == '__main__':
    unittest.main()