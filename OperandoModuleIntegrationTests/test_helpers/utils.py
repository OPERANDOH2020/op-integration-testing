class ClientError(Exception):
    """Response code between 400 and 499"""
    
class ServerError(Exception):
    """Response code between 500 and 599"""

class ServiceTicketValidationError(Exception):
    """An error caused when a ST is not valid for the specified service"""

def raise_on_error(response):
    """Raises an appropriate exception if the response is an error"""
    if 400 <= response.status_code < 500:
        raise ClientError(response.text)
    if 500 <= response.status_code < 600:
        raise ServerError(response.text)
