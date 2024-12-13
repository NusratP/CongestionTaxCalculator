# Changelog

## [Unreleased]

### Implemented

1. Created an asp.net application containing swagger page. This will handle the startup.
2. Fixed logic : 
	The logic of iterating on the dates in GetTax() method was not making sense so I wrote my own.
 	optimized code in IsTollFreeVehicle()
	OldLogic in GetTollFee() is fixed and now it is also using the data from the database.

3. Created DataAccess Project for getting Time and toll fee from the database using entity framework.
4. created UnitTests Project for test cases.	


### TODO: 
1. Bonus scenario:  Gothenburg region is considered for calculating TollFees. I haven't created entity/table for cities.
2. Logging
