using HospitalDemo.Data;
using HospitalDemo.Models.Bill;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillController : Controller
    {
        Random rng = new Random();
        private readonly HospitalDbContext dbContext;
        public BillController(HospitalDbContext dbContext)
        {
            this.dbContext=dbContext;
        }

        [HttpGet]
        [Route("get_all_bill")]
        public async Task<IActionResult> Get_bill()
        {
            return Ok(await dbContext.bill.ToListAsync());
        }

        [HttpGet]
        [Route("get_by_id/{id}")]
        public async Task<IActionResult> Get_bill([FromRoute]int id)
        {
            var data = await dbContext.bill.FirstOrDefaultAsync(b => b.id == id);
            if(data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpPost]
        [Route("Add_bill")]
        public async Task<IActionResult> Add_bill([FromBody]Bill_Request_Model b)
        {
            var bill_to_add = new Bill();
            bill_to_add.id = rng.Next(1, 1001);
            bill_to_add.created_time = DateTime.UtcNow;
            bill_to_add.updated_time = DateTime.UtcNow;
            bill_to_add.patient_id = b.patient_id;
            bill_to_add.patient_name = b.patient_name;
            bill_to_add.patient_phone = b.patient_phone;
            bill_to_add.total_amount = b.total_amount;
            bill_to_add.created_user_id = 0;
            bill_to_add.updated_user_id = 0;
            bill_to_add.printed_or_drafted = b.printed_or_drafted;
            bill_to_add.patient_address = b.patient_address;
            bill_to_add.is_cancelled = b.is_cancelled;

            await dbContext.bill.AddAsync(bill_to_add);
            await dbContext.SaveChangesAsync();
            return Ok(bill_to_add);
        }

        [HttpPut]
        [Route("update_bill/{id}")]
        public async Task<IActionResult> Update_bill([FromRoute]int id, [FromBody] Bill_Request_Model b)
        {
            var bill_to_update = await dbContext.bill.FirstOrDefaultAsync(b => b.id == id);
            if( bill_to_update == null)
            {
                return NotFound();
            }
            
            
            bill_to_update.updated_time = DateTime.UtcNow;
            bill_to_update.patient_id = b.patient_id;
            bill_to_update.patient_name = b.patient_name;
            bill_to_update.patient_phone = b.patient_phone;
            bill_to_update.total_amount = b.total_amount;
           
            bill_to_update.printed_or_drafted = b.printed_or_drafted;
            bill_to_update.patient_address = b.patient_address;
            bill_to_update.is_cancelled = b.is_cancelled;

            dbContext.bill.Update(bill_to_update);
            await dbContext.SaveChangesAsync();
            return Ok(bill_to_update);
        }
        [HttpPut]
        [Route("bulk update")]
        public async Task<IActionResult> Update_bulk([FromBody] List<Bill_Bulk_Update_Model> bulk_data)
        {
            foreach(var b in bulk_data)
            {
                var bill_to_update = await dbContext.bill.FirstOrDefaultAsync(b => b.id == b.id);
                if (bill_to_update == null)
                {
                    return NotFound();
                }


                bill_to_update.updated_time = DateTime.UtcNow;
                bill_to_update.patient_id = b.patient_id;
                bill_to_update.patient_name = b.patient_name;
                bill_to_update.patient_phone = b.patient_phone;
                bill_to_update.total_amount = b.total_amount;

                bill_to_update.printed_or_drafted = b.printed_or_drafted;
                bill_to_update.patient_address = b.patient_address;
                bill_to_update.is_cancelled = b.is_cancelled;

                dbContext.bill.Update(bill_to_update);
                await dbContext.SaveChangesAsync();
            }
            return Ok("bulk_update complete");
        }

        [HttpDelete]
        [Route("delete_bill/{id}")]
        public async Task<IActionResult> Delete_bill([FromRoute] int id)
        {
            var bill_to_delete = await dbContext.bill.FirstOrDefaultAsync(b => b.id == id);
            if(bill_to_delete == null)
            {
                return NotFound();
            }
            dbContext.bill.Remove(bill_to_delete);
            await dbContext.SaveChangesAsync();
            return Ok(bill_to_delete);
        }

        [HttpDelete]
        [Route("bulk_delete")]
        public async Task<IActionResult> Delete_bill([FromBody]List<int> id_list)
        {

            foreach(var id in id_list)
            {
                var bill_to_delete = await dbContext.bill.FirstOrDefaultAsync(b => b.id == id);
                if (bill_to_delete == null)
                {
                    return NotFound();
                }
                dbContext.bill.Remove(bill_to_delete);
                await dbContext.SaveChangesAsync();
            }
            return Ok("bulk_delete_complete");
        }

        //[HttpDelete]
        //[Route("bulk_delete")]
        //public async Task<IActionResult> Delete_bill([FromBody] List<int> id_list)
        //{
        //    foreach (var id in id_list)
        //    {

        //    }
        //}

       
    }
}
