using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace MyProjectPlugins
{
    public class TaskCreate : IPlugin
    {

        //int tax; // class variable 
        //public TaskCreate(string unSecureConfig, string secureConfig) //secure and unsecure config in the plugin update manager. creating constructor for dynamics datas
        //{
        //    tax = Convert.ToInt32(unSecureConfig);
        //}
        public void Execute(IServiceProvider serviceProvider)
        {
            // Extract the tracing service for use in debugging sandboxed plug-ins.  
            // If you are not registering the plug-in in the sandbox, then you do  
            // not have to add any tracing service related code.  
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the organization service reference which you will need for  
            // web service calls.  
            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            //IOrganizationService adminservice = serviceFactory.CreateOrganizationService(new Guid("")); Impersonation plugin



            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity rgv_customer = (Entity)context.InputParameters["Target"]; //new object of entity class


                try
                {
                    //string key = context.SharedVariables["key1"].ToString(); //getting the key value from previous plugin 



                    Entity taskCreate = new Entity("task"); //"task" is the name of the entity/table. Mine one is Customer and Offers
                    taskCreate.Attributes.Add("subject", "Follow up"); //subject is the field name and follow up is the value sent in that form
                    taskCreate.Attributes.Add("description", "Please follow up with contact");

                    //date
                    taskCreate.Attributes.Add("scheduledend", DateTime.Now.AddDays(2));

                    //optionset
                    taskCreate.Attributes.Add("prioritycode", new OptionSetValue(2));

                    //parent record or lookup
                    taskCreate.Attributes.Add("regardingobjectid", rgv_customer.ToEntityReference());

                    Guid taskGuid = service.Create(taskCreate);

                    //adminservice.Create(taskCreate); // Impersonation plugin 

                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in MyPlug-in.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("MyPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}

 