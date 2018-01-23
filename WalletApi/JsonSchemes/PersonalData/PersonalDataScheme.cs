using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LykkeAutomation.Api.JsonSchemes.PersonalData
{
    public class PersonalDataSchemes
    {

        public JSchema PersonalDataResponseSchema = JSchema.Parse(
            @"{
  'type': 'object',
  '$schema': 'http://json-schema.org/draft-03/schema',
  'required': false,
  'properties': {
    'Error': {
      'type': 'null',
      'required': true
    },
    'Result': {
      'type': 'object',
      'required': true,
      'properties': {
        'Address': {
          'type': 'null',
          'required': true
        },
        'City': {
          'type': 'string',
          'required': true
        },
        'Country': {
          'type': 'string',
          'required': true
        },
        'Email': {
          'type': 'string',
          'required': false
        },
        'FirstName': {
          'type': 'null',
          'required': true
        },
        'FullName': {
          'type': 'string',
          'required': true
        },
        'LastName': {
          'type': 'null',
          'required': false
        },
        'Phone': {
          'type': 'string',
          'required': false
        },
        'Zip': {
          'type': 'null',
          'required': true
        }
      },
      'additionalProperties': false
    }
  }
}"
);
    }
}
