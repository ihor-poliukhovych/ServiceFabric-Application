# ServiceFabric-Application

### 1. Extract variables
POST http://localhost:8989/api/values

BODY "(x+max(x1,5))/d–sqrt(z)+b*CalculateSalary(\"Ivanov\",-1+x)"

### 2. Get progress
POST http://localhost:8989/api/values/progress

BODY "(x+max(x1,5))/d–sqrt(z)+b*CalculateSalary(\"Ivanov\",-1+x)"

### 2. Replace Variables
PUT http://localhost:8989/api/values

BODY
```json
{  
   "Expression":"(x+max(x1,5))/d–sqrt(z)+b*CalculateSalary(\"Ivanov\",-1+x)",
   "Values":[  
      {  
         "Key":"x",
         "Value":"1"
      },
	  {  
         "Key":"x1",
         "Value":"2"
      }
   ]
}
```