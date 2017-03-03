import json
import requests
import sys
from xml.etree import ElementTree
from test_helpers.utils import raise_on_error

class UnableToCreateRegulationError(Exception):
    """The response did not contain the regulation id"""

class PoliciesDatabaseService(object):

    post_url = "http://integration.operando.esilab.org:8096/operando/core/pdb/regulations"
    service_id = "pdb/regulations/.*"

    def __init__(self, timeout):
        self.timeout = timeout

    def create_regulation(self, data, service_ticket):
        """Creates a regulation using the data. Returns (response, reg_id) 
        where response is the response, and reg_id is the newly-created regulation id
        """

        headers = {"service-ticket": service_ticket}
        response = requests.post(self.post_url, json=data, headers=headers, timeout=self.timeout)

        return response

    def get_reg_id_from_response(self, response):
        content = ElementTree.fromstring(response.text)
        response_message = content.find("message")
        data = json.loads(response_message.text)
        return data["reg_id"]

    def create_RegulationsPost_model(self, 
                                     legislation_sector="Default test sector", private_information_source=None,
                                     private_information_type=None, action=None, required_consent=None):
        model = {
            "legislation_sector": legislation_sector
        }
        if private_information_source is not None: 
            model["private_information_source"] = private_information_source
        if private_information_type is not None: 
            model["private_information_type"] = private_information_type
        if action is not None: 
            model["action"] = action
        if required_consent is not None: 
            model["required_consent"] = action
        
        return model