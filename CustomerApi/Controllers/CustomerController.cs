using CustomerAPI.Models;
using CustomerAPI.Repositories;
using CustomerAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {       
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Route("customers")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var customers = await _customerService.GetAllAsync();
                if (customers == null)
                {
                    return NotFound();
                }

                return Ok(customers);
            }
            catch (Exception)
            {
                return BadRequest();
            }           
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync(int? customerId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Not a valid model");

                if (!customerId.HasValue)
                {
                    return BadRequest();
                }
                var customer = await _customerService.GetAsync(customerId);
                if (customer == null)
                {
                    return NotFound();
                }

                return Ok(customer);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddAsync(Customer customerInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Not a valid model");

                if (customerInfo == null)
                    return BadRequest();

                //Check if customer exists in the database
                var existingCustomer = await _customerService.GetAsync(customerInfo.CustomerId);

                if(existingCustomer != null)
                {
                    BadRequest();
                }
                await _customerService.AddAsync(customerInfo);             

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }           
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Customer customerInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Not a valid model");

                if (customerInfo == null)
                {
                    return BadRequest();
                }

                //Check if the customer exists in the database
                Customer existingCustomerInfo = await _customerService.GetAsync(customerInfo.CustomerId);
                if (existingCustomerInfo == null)
                {
                    return NotFound();
                }
                //Replace with new values
                existingCustomerInfo.FirstName = customerInfo.FirstName;
                existingCustomerInfo.LastName = customerInfo.LastName;
                existingCustomerInfo.DateOfBirth = customerInfo.DateOfBirth;

                //Update customer with the new values
                await _customerService.Update(existingCustomerInfo);

                return Ok();
               
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete]        
        public async Task<IActionResult> DeleteAsync(int? customerId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Not a valid model");

                if (!customerId.HasValue)
                {
                    return BadRequest();
                }
                //Check if the customer exists
                var existingCustomer = await _customerService.GetAsync(customerId);
                if (existingCustomer == null)
                {
                    return NotFound();
                }

                //Update customer with the new values
                await _customerService.Remove(existingCustomer);

                return Ok();
               
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
