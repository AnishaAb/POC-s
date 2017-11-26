using Final.NHibernateModels;
using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Final.Controllers
{
    public class CustomerUsingSPController : Controller
    {
        //Crud operations on Customer using SP 
        public int customerLength = 0;
        public ActionResult GetCustomerUsingSp()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var customerList = session.CreateSQLQuery(@"exec sp_GetCustomers").SetResultTransformer(new AliasToBeanResultTransformer(typeof(Customer)));
                IList<Customer>  customers = customerList.List<Customer>();
                Session["CustomerCount"] = customers.Count;
                return View(customers);
            }
        }

        [HttpPost]
        public ActionResult GetCustomerByLastName(string lastName)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var customerList = session.CreateSQLQuery(@"exec GetCustomerByLastName :lname").SetParameter("lname", lastName);
                customerList = customerList.SetResultTransformer(Transformers.AliasToBean(typeof(Customer)));
                IList<Customer> customers = customerList.List<Customer>();
                return View(customers);
            }
        }

        [HttpGet]
        public ActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCustomer(Customer customer)
        {
            //Need to re-work on this
            customerLength = Convert.ToInt16(Session["CustomerCount"]);
            customer.Id = customerLength + 1;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var newCustomer = session.CreateSQLQuery(@"exec spCreateCustomer :Id ,:FirstName, :LastName,:Status")
                                        .SetParameter("Id",customer.Id)
                                        .SetParameter("FirstName", customer.FirstName)
                                        .SetParameter("LastName", customer.LastName)
                                        .SetParameter("Status", customer.Status)
                                        .ExecuteUpdate();
            }
            return RedirectToAction("GetCustomerUsingSp");
        }

        [HttpGet]
        public ActionResult EditCustomer(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var customer = session.CreateSQLQuery(@"exec GetCustomerById :id")
                                        .SetParameter("id", id);
                customer = customer.SetResultTransformer(new AliasToBeanResultTransformer(typeof(Customer)));
                IList<Customer> selectedCustomer = customer.List<Customer>();
                return View(selectedCustomer.First());
            }
        }

        [HttpPost]
        public ActionResult EditCustomer(int id, Customer customer)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                session.CreateSQLQuery(@"exec spUpdateCustomer :id, :firstName,:lastName,:status")
                        .SetParameter("id", id)
                        .SetParameter("firstName", customer.FirstName)
                        .SetParameter("lastName", customer.LastName)
                        .SetParameter("status", customer.Status)
                        .ExecuteUpdate();
            }
            return RedirectToAction("GetCustomerUsingSp");
        }

        [HttpGet]
        public ActionResult DeleteCustomer(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var customer = session.CreateSQLQuery(@"exec GetCustomerById :id")
                                        .SetParameter("id", id);
                customer = customer.SetResultTransformer(new AliasToBeanResultTransformer(typeof(Customer)));
                IList<Customer> selectedCustomer = customer.List<Customer>();
                return View(selectedCustomer.First());
            }
        }

        [HttpPost]
        public ActionResult DeleteCustomer(int id, Customer customer)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                session.CreateSQLQuery(@"exec spDeleteCustomer :id")
                         .SetParameter("id", id)
                         .ExecuteUpdate();
            }
            return RedirectToAction("GetCustomerUsingSp");
        }

    }
}