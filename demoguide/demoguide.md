[comment]: <> (please keep all comment items at the top of the markdown file)
[comment]: <> (please do not change the ***, as well as <div> placeholders for Note and Tip layout)
[comment]: <> (please keep the ### 1. and 2. titles as is for consistency across all demoguides)
[comment]: <> (section 1 provides a bullet list of resources + clarifying screenshots of the key resources details)
[comment]: <> (section 2 provides summarized step-by-step instructions on what to demo)


[comment]: <> (this is the section for the Note: item; please do not make any changes here)
***
### Product Catalog backend MCP Server with Copilot Studio

This scenario deploys a dotnet application, acting as a backend MCP Server for an imaginary Product Catalog Inventory Generative AI application. The demoguide explains the different steps on how to use this with **Copilot Studio**, but it could be used with any other scenario, supporting MCP Servers.

<div style="background: lightgreen; 
            font-size: 14px; 
            color: black;
            padding: 5px; 
            border: 1px solid lightgray; 
            margin: 5px;">

**Note:** The app can be deployed and run in 2 different ways, either as a local app on your development workstation, or as a published web app in Azure App Services. See the README.md for more details on how to get either setup working. The steps in this demoguide assume the MCP Server is running in Azure App Service. 
</div>

<img src="https://raw.githubusercontent.com/petender/demo-mcp-productcatalog/refs/heads/main/demoguide/CopilotStudio_Retail_Inventory_Agent.png" alt="Copilot Studio Retail Inventory Agent with MCP Server backend" style="width:70%;">
<br></br>

<div style="background: lightgreen; 
            font-size: 14px; 
            color: black;
            padding: 5px; 
            border: 1px solid lightgray; 
            margin: 5px;">

**Note:** Below demo steps should be used **as a guideline** for doing your own demos. Please consider contributing to add additional demo steps.
</div>

***
### 1. What Resources are getting deployed

The following resources are getting deployed:

* RG-<azd-env-name> : The Resource Group using the AZD env name you specified
* %uniquestring%app-plan : Azure App Service Plan (Free tier)
* %uniquestring%app : Azure App Service running the MCP Server application 

<img src="https://raw.githubusercontent.com/petender/demo-mcp-productcatalog/refs/heads/main/demoguide/ResourceGroup_Overview.png" alt="MCP Server App Service Resource Group" style="width:70%;">
<br></br>


### 2. What can I demo from this scenario after deployment

#### Copilot Studio Retail Inventory Agent

1. From [Copilot Studio](https://copilotstudio.microsoft.com), **create a new agent** using the **configure** option. At a minimum, provide a Name and Description. Click **Create Agent**

<img src="https://raw.githubusercontent.com/petender/demo-mcp-productcatalog/refs/heads/main/demoguide/Create_CopilotStudio_Agent.png" alt="Create CopilotStudio Agent" style="width:70%;">
<br></br>

2. Select the newly created Agent, to open its configuration/settings blade. Navigate to **Tools**. Click **Add a Tool**.

<img src="https://raw.githubusercontent.com/petender/demo-mcp-productcatalog/refs/heads/main/demoguide/Add_Tool.png" alt="Add Tool" style="width:70%;">
<br></br>

3. From the **Add tool** popup window, showing different tools and connectors, click the **+ New Tool** button.

<img src="https://raw.githubusercontent.com/petender/demo-mcp-productcatalog/refs/heads/main/demoguide/Add_Tool_Popup.png" alt="Add Tool Popup" style="width:70%;">
<br></br>

4. From the **Add tool** popup window, showing different tool setup options, select **Model Context Protocol**

<img src="https://raw.githubusercontent.com/petender/demo-mcp-productcatalog/refs/heads/main/demoguide/Add_Tool_Popup_MCP.png" alt="Add Tool Popup" style="width:70%;">
<br></br>

5. In the **Add a Model Context Protocol server (Preview)**, complete the necessary fields with the correct information:

- **Server Name**: a short, clear name for the MCP Server. e.g. mcpproddemo
- **Server Description**: short description what the MCP Server is doing. e.g. Product Catalog MCP Demo
- **Server URL**: the Azure App Service public URL (e.g. %uniquestring%.azurewebsites.net) (if running this scenario from your local machine, grab the public url from the devtunnel steps)
- **Auth**: None (although it should be either of the other options in a production scenario!)

Confirm the creation of the MCP Server with those details. 

<img src="https://raw.githubusercontent.com/petender/demo-mcp-productcatalog/refs/heads/main/demoguide/MCP_Server_Config.png" alt="MCP Server Setup" style="width:70%;">
<br></br>

6. The new MCP Server connection shows up in the list of **Tools**. Click on its **Name** to open the **details** blade. Scroll down to the **Tools** section of the details blade, showing the different 'Actions' (Functionalities) you have available through the MCP Server. 

<img src="https://raw.githubusercontent.com/petender/demo-mcp-productcatalog/refs/heads/main/demoguide/MCP_Tool_Details.png" alt="MCP Tools Tool Details" style="width:70%;">
<br></br>

(for more details on these tools/functionalities, check the README.md)

7. Open the **Test your agent**. Wait for it to greet you with a descriptive message, saying it is a virtual assistant.

<img src="https://raw.githubusercontent.com/petender/demo-mcp-productcatalog/refs/heads/main/demoguide/Test_Your_Agent.png" alt="Test Your Agent" style="width:70%;">
<br></br>

8. Interact with the virtual assistant by running a prompt. For example, you could ask 

```bash
share details about the desk lamp
```




[comment]: <> (this is the closing section of the demo steps. Please do not change anything here to keep the layout consistant with the other demoguides.)
<br></br>
***
<div style="background: lightgray; 
            font-size: 14px; 
            color: black;
            padding: 5px; 
            border: 1px solid lightgray; 
            margin: 5px;">

**Note:** This is the end of the current demo guide instructions.
</div>




