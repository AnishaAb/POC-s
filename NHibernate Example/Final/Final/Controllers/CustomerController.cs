using Final.NHibernateModels;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Final.Controllers
{
    public class CustomerController : Controller
    {
        // Fetch: Customer Details using table/view 
        public ActionResult FetchCustomerDetails()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                List<Customer> customers = session.Query<Customer>().ToList();
                return View(customers);
            }
        }

        public ActionResult GetCustomerById(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                Customer customer = session.Get<Customer>(id);
                return View(customer);
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
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(customer);
                    transaction.Commit();
                }
            }
            return RedirectToAction("FetchCustomerDetails");
        }

        [HttpGet]
        public ActionResult EditCustomer(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                Customer customer = session.Get<Customer>(id);
                return View(customer);
            }
        }

        [HttpPost]
        public ActionResult EditCustomer(int id, Customer customer)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                Customer existingCustomer = session.Get<Customer>(id);
                existingCustomer.FirstName = customer.FirstName;
                existingCustomer.LastName = customer.LastName;
                existingCustomer.Status = customer.Status;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(existingCustomer);
                    transaction.Commit();
                }
            }
            return RedirectToAction("FetchCustomerDetails");
        }

        [HttpGet]
        public ActionResult DeleteCustomer(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var customer = session.Get<Customer>(id);
                return View(customer);
            }
        }

        [HttpPost]
        public ActionResult DeleteCustomer(int id, Customer customer)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(customer);
                    transaction.Commit();
                }
            }
            return RedirectToAction("FetchCustomerDetails");
        }
    }
}