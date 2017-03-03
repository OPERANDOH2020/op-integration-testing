import requests
from test_helpers.aapi_service import AuthenticationApiService
from test_helpers.AuthenticatableTestCase import AuthenticatableTestCase
import unittest

class LogDbTests(AuthenticatableTestCase):
   
    post_url = "http://integration.operando.esilab.org:8090/operando/core/ldb/log/logTicket"
    search_url = "http://integration.operando.esilab.org:8091/operando/core/ldbsearch/log/search"
    
    post_service_id = "/operando/core/ldb"
    search_service_id = "/operando/core/ldbsearch/log/search"

    def _post_logdb(self, data, service_ticket, post_as_json=True):
        headers = {"service-ticket": service_ticket}

        kwargs = {
            "headers": headers,
            "timeout": self.TIMEOUT,
        }

        if post_as_json:
            kwargs["json"] = data
        else:
            kwargs["data"] = data

        response = requests.post(self.post_url, **kwargs)
        
        print response.text
        return response

    def _get_logdbsearch(self, service_ticket):
        headers = {"service-ticket": service_ticket}
        response = requests.get(self.search_url, headers=headers, timeout=self.TIMEOUT)
        print response.text
        return response

    def _create_LogRequest_ticket(self, 
                                  userId="001", requesterType="MODULE", requesterId="1001", logPriority="NORMAL", 
                                  logLevel="INFO", title="Privacy settings discrepancy", keywords=None,
                                  description="The privacy settings for user 001 with OSP 12 are not as required. This requires action."):
        ticket = {
            "userId": userId,
            "requesterType": requesterType,
            "requesterId": requesterId,
            "logPriority": logPriority,
            "logLevel": logLevel,
        }
        if title is not None: 
            ticket["title"] = title
        if description is not None: 
            ticket["description"] = description
        if keywords is not None: 
            ticket["keywords"] = keywords
        
        return ticket
    
    def _get_post_service_ticket(self):
        return super(LogDbTests, self)._get_service_ticket_for(self.post_service_id)

    def _get_search_service_ticket(self):
        return super(LogDbTests, self)._get_service_ticket_for(self.search_service_id)

    def test_post_invalid_service_ticket_returns_403(self):
        invalid_st = "invalid service ticket!"
        
        model = self._create_LogRequest_ticket()
        response = self._post_logdb(model, invalid_st)
        
        self.assertEqual(403, response.status_code)
        
    def test_post_successfull_post_returns_200(self):
        st = self._get_post_service_ticket()

        model = self._create_LogRequest_ticket()
        response = self._post_logdb(model, st)

        self.assertEqual(200, response.status_code)
    
    def test_post_invalid_data_returns_400(self):
        st = self._get_post_service_ticket()
        
        model = {"invalid": "model"}
        response = self._post_logdb(model, st)

        self.assertEqual(400, response.status_code)

    def test_post_body_not_json_returns_415(self):
        st = self._get_post_service_ticket()
        
        model = self._create_LogRequest_ticket()
        
        model_as_key_value_pairs = "\r\n".join("{}: {}".format(k, v) for k, v in model.items())

        response = self._post_logdb(model_as_key_value_pairs, st, post_as_json=False)

        self.assertEqual(415, response.status_code)
    
    def test_search_valid_request_returns_200_with_text(self):
        st = self._get_search_service_ticket()
        
        response = self._get_logdbsearch(st)
        
        self.assertEqual(200, response.status_code)
        self.assertIsNotNone(response.text)
        self.assertNotEqual("", response.text)
    
    def test_search_invalid_service_ticket_returns_403(self):
        invalid_st = "invalid service ticket!"
        
        response = self._get_logdbsearch(invalid_st)
        
        self.assertEqual(403, response.status_code)
            
if __name__ == '__main__':
    unittest.main()