<h1>Student Course System API</h1>
Simple .net 8 WebApi project for managing Courses and Students.

The API uses a simple API Key for security which much be sent with each request in a Header named 'x-api-key'.
The API Key is specified in AppSettings.json. (In production this would be moved either Secrets.json or an Environment Variable for improved security).

<strong>API Key:</strong></strong>
<code>ihqvkmciorvdwubphzywyefcmfxzvobc
</code>

<h2>Data Access Layer</h2>
The Api uses an SqlLite file which is included with the project. Its located at /database/studentsystem.db within the project folder.

<h2>Running the Project</h2>
<ol>
  <li>Open the project with V2022</li>
  <li>Run the project using the Debug or Release profile, using https.</li>
  <li>SwaggerUI can be used to test the API end points. Before testing with Swagger, specify the API key by clicking the 'Authorize' button.</li>
</ol>


