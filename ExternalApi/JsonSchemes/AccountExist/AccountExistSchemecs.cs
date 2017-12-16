using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LykkeAutomation.Api.JsonSchemes.AccountExist
{
    public class AccountExistSchemes
    {
        public JSchema AuthResponseScheme = JSchema.Parse(
            @"
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
      'type': ['object', 'null']
    }, 
    'Result': {
      'additionalProperties': false, 
      'id': '/properties/Result', 
      'properties': {
        'IsEmailRegistered': {
          'id': '/properties/Result/properties/IsEmailRegistered', 
          'type': 'boolean'
        }
      }, 
      'required': [
        'IsEmailRegistered'
      ], 
      'type': 'object'
    }
  }, 
  'required': [
    'Result', 
    'Error'
  ], 
  'type': ['object', 'null']
}
");
    }
}
