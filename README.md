Database
--------
	-	MS-SQL server, with the database imported from the backup attached in the project.
	
Back-end
--------
	-	You will need dotnet core to start the API server.
	
	-	You can build the application
		-	by using "dotnet run" which starts the server as well
		-	from the visual studio project
		
	-	You can start the server 
		-	by a simple "dotnet run" command
		-	by starting a developer IIS server from visual studio
		-	by hosting the dlls generated from the build on an IIS server
	
Front-end
---------
	
	environment setup
	-----------------
	-	You will have to update the API port number in the 
		environment.ts and environment.prod.ts files as required to be able to reach the APIs.
	-	You will have to run "npm install" to fetch all the dependencies in the project path "code/AngularUI".
	
	
	-	You will need npm, node and angular
	-	You can install angular with "npm install angular -g" command
	
	-	You can build static files of the application with "ng build --aot --prod" command in the project folder.
		And host these static files on apache or nginx or IIS server.
		
												------ or ------
												
		You can start angular developer server with "ng serve -o" command in the project folder.
