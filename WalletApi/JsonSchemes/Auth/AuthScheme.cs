using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LykkeAutomation.Api.JsonSchemes.Auth
{
    public class AuthScheme
    {
        public JSchema AuthResponseScheme = JSchema.Parse(@"
      {
  '$schema': 'http://json-schema.org/draft-06/schema#', 
  'additionalProperties': false, 
  'definitions': {}, 
  'id': 'http://example.com/example.json', 
  'properties': {
    'Error': {
      'additionalProperties': false, 
      'id': '/properties/Error', 
      'properties': {
        'Code': {
          'id': '/properties/Error/properties/Code', 
          'type': 'string'
        }, 
        'Field': {
          'id': '/properties/Error/properties/Field', 
          'type': 'string'
        }, 
        'Message': {
          'id': '/properties/Error/properties/Message', 
          'type': 'string'
        }
      }, 
      'required': [
        'Field', 
        'Message', 
        'Code'
      ], 
      'type': ['object','null']
    }, 
    'Result': {
      'additionalProperties': false, 
      'id': '/properties/Result', 
      'properties': {
        'CanCashInViaBankCard': {
          'id': '/properties/Result/properties/CanCashInViaBankCard', 
          'type': 'boolean'
        }, 
        'IsUserFromUSA': {
          'id': '/properties/Result/properties/IsUserFromUSA', 
          'type': 'boolean'
        }, 
        'KycStatus': {
          'id': '/properties/Result/properties/KycStatus', 
          'type': 'string'
        }, 
        'NotificationsId': {
          'id': '/properties/Result/properties/NotificationsId', 
          'type': 'string'
        }, 
        'PersonalData': {
          'additionalProperties': false, 
          'id': '/properties/Result/properties/PersonalData', 
          'properties': {
            'Address': {
              'id': '/properties/Result/properties/PersonalData/properties/Address', 
              'type': ['null', 'string']
            }, 
            'City': {
              'id': '/properties/Result/properties/PersonalData/properties/City', 
              'type': ['null', 'string']
            }, 
            'Country': {
              'id': '/properties/Result/properties/PersonalData/properties/Country', 
              'type': 'string'
            }, 
            'Email': {
              'id': '/properties/Result/properties/PersonalData/properties/Email', 
              'type': 'string'
            }, 
            'FirstName': {
              'id': '/properties/Result/properties/PersonalData/properties/FirstName', 
              'type': ['null', 'string']
            }, 
            'FullName': {
              'id': '/properties/Result/properties/PersonalData/properties/FullName', 
              'type': 'string'
            }, 
            'LastName': {
              'id': '/properties/Result/properties/PersonalData/properties/LastName', 
              'type': ['null', 'string']
            }, 
            'Phone': {
              'id': '/properties/Result/properties/PersonalData/properties/Phone', 
              'type': 'string'
            }, 
            'Zip': {
              'id': '/properties/Result/properties/PersonalData/properties/Zip', 
              'type': ['null', 'string']
            }
          }, 
          'required': [
            'City', 
            'Zip', 
            'FirstName', 
            'LastName', 
            'Address', 
            'Phone', 
            'Country', 
            'FullName', 
            'Email'
          ], 
          'type': 'object'
        }, 
        'PinIsEntered': {
          'id': '/properties/Result/properties/PinIsEntered', 
          'type': 'boolean'
        }, 
        'SwiftDepositEnabled': {
          'id': '/properties/Result/properties/SwiftDepositEnabled', 
          'type': 'boolean'
        }, 
        'Token': {
          'id': '/properties/Result/properties/Token', 
          'type': 'string'
        }
      }, 
      'required': [
        'CanCashInViaBankCard', 
        'SwiftDepositEnabled', 
        'Token', 
        'IsUserFromUSA', 
        'PersonalData', 
        'KycStatus', 
        'NotificationsId', 
        'PinIsEntered'
      ], 
      'type': 'object'
    }
  }, 
  'required': [
    'Result', 
    'Error'
  ], 
  'type': 'object'
}
");
    }
}
