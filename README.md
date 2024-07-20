<h1>Student Course System API</h1>
Simple .net 8 WebApi project for managing Courses and Students.

The API uses a simple API Key for security which much be sent with each request in a Header named 'x-api-key'.
The API Key is specified in AppSettings.json. (In production this would be moved either Secrets.json or an Environment Variable for improved security).

<strong>API Key:</strong></strong>
<code>ihqvkmciorvdwubphzywyefcmfxzvobc
</code>

<h2>Data Access Layer</h2>
The Api uses an SqlLite file which is included with the project. Its located at /database/studentsystem.db within the project folder.

<h2>Vue3 Front End Project</h2>
A current build of the sample Vue3 Front End can be found on GitHubPages https://neutron2k24.github.io/SystemApiSystemVueFrontEnd/
Note that The StudentApiSystem must be running locally at https://localhost:7060/api in order for it to function.

<h2>Running the Project</h2>
<ol>
  <li>Open the project with V2022</li>
  <li>Run the project using the Debug or Release profile, using https.</li>
  <li>SwaggerUI can be used to test the API end points. Before testing with Swagger, specify the API key by clicking the 'Authorize' button.</li>
</ol>

You can also add the Vue3 Front End project from repo https://github.com/neutron2k24/SystemApiSystemVueFrontEnd and run both projects locally. Refer to the readme.md for the front end project for required npm packages.

