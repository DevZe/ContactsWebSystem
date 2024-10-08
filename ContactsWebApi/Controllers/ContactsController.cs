﻿using ContactsWebApi.Data;
using ContactsWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ContactModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactModel>>> GetContacts()
        {
            var contacts = await _context.Contacts.ToListAsync();
            //This is a long way of getting Address of the contact, might have a better way
            foreach (ContactModel contact in contacts)
            {
                contact.Address = await _context.Address.FirstOrDefaultAsync(i => i.Id == contact.Id);
            }

            return contacts;
        }

        // GET: api/ContactModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactModel>> GetContactModel(int id)
        {
            var contactModel = await _context.Contacts
                        .Include(i => i.Address)
                        .FirstOrDefaultAsync(i => i.Id == id);

            if (contactModel == null)
            {
                return NotFound();
            }


            return contactModel;
        }

        //PUT: api/ContactModels/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactModel(int id, ContactModel contactModel)
        {
            if (id != contactModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(contactModel).State = EntityState.Modified;
            _context.Entry(contactModel.Address).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactModelExists(id))
                {
                    return NotFound();
                }
                else if (!AddressModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ContactModels

        [HttpPost]
        public async Task<ActionResult<ContactModel>> PostContactModel(ContactModel contactModel)
        {
            _context.Contacts.Add(contactModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContactModel", new { id = contactModel.Id }, contactModel);
        }

        // DELETE: api/ContactModels/5
        [HttpDelete]
        public async Task<IActionResult> DeleteContactModel(int id)
        {
            var contactModel = await _context.Contacts.FindAsync(id);
            if (contactModel == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contactModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactModelExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }

        private bool AddressModelExists(int id)
        {
            return _context.Address.Any(e => e.Id == id);
        }
    }
}
