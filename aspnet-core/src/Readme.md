Server App for the first Task;
It uses Ef Core and SQl server as Persistance layer. 
If you want to change it  remove the DAL Project with your implementation and update the applogicRegisterModule.
The connection string is in the appsettings.development.json; it uses the built in sql server expres so  it should 
work on your machine just use the script in the DAL project;
for running the project run the run.ps in the HttpApi.host project;