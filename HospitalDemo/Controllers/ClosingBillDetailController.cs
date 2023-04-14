﻿using HospitalDemo.Data;
using HospitalDemo.Models.Bill;
using HospitalDemo.Models.ClosingBillDetail;
using HospitalDemo.Models.DailyClosing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClosingBillDetailController : Controller
    {
        Random rng = new Random();
        private readonly HospitalDbContext dbContext;
        public ClosingBillDetailController(HospitalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("get_all")]
        public async Task<IActionResult> Get_all()
        {
            return Ok(await dbContext.closingbilldetail.ToListAsync());
        }

        [HttpGet]
        [Route("get_by_id/{id}")]
        public async Task<IActionResult> Get_bill([FromRoute] int id)
        {
            var data = await dbContext.closingbilldetail.FirstOrDefaultAsync(b => b.id == id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpPost]
        [Route("Add_dailclosing")]
        public async Task<IActionResult> Add_dailyclosing([FromBody] ClosingBillDetal_Request_Model dc)
        {
            var cd_to_add = new Closingbilldetail();

            cd_to_add.id = rng.Next(1, 1001);
            cd_to_add.created_time = DateTime.UtcNow;
            cd_to_add.updated_time = DateTime.UtcNow;
            cd_to_add.daily_closing_id = dc.daily_closing_id;
            cd_to_add.bill_id = dc.bill_id;
            cd_to_add.amount = dc.amount;
            cd_to_add.created_user_id = 0;
            cd_to_add.updated_user_id = 0;

            await dbContext.closingbilldetail.AddAsync(cd_to_add);
            await dbContext.SaveChangesAsync();
            return Ok(cd_to_add);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> Update_dailyclosing([FromRoute] int id, [FromBody] ClosingBillDetal_Request_Model dc)
        {
            var dc_to_udpate = await dbContext.closingbilldetail.FirstOrDefaultAsync(d => d.id == id);
            if (dc_to_udpate == null)
            {
                return NotFound();
            }


            dc_to_udpate.updated_time = DateTime.UtcNow;
            dc_to_udpate.daily_closing_id = dc.daily_closing_id;
            dc_to_udpate.bill_id = dc.bill_id;
            dc_to_udpate.amount = dc.amount;

            dbContext.closingbilldetail.Update(dc_to_udpate);
            await dbContext.SaveChangesAsync();

            return Ok(dc_to_udpate);
        }

        [HttpPut]
        [Route("bulk update")]
        public async Task<IActionResult> Update_bulk([FromBody] List<ClosingBillDetail_Bulk_Update_Model> bulk_data)
        {
            foreach (var bu in bulk_data)
            {
                var dc_to_udpate = await dbContext.closingbilldetail.FirstOrDefaultAsync(d => d.id == bu.id);
                if (dc_to_udpate == null)
                {
                    return NotFound();
                }


                dc_to_udpate.updated_time = DateTime.UtcNow;
                dc_to_udpate.daily_closing_id = bu.daily_closing_id;
                dc_to_udpate.bill_id = bu.bill_id;
                dc_to_udpate.amount = bu.amount;

                dbContext.closingbilldetail.Update(dc_to_udpate);
                await dbContext.SaveChangesAsync();
            }
            return Ok("bulk_update complete");
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete_dailyclosing([FromRoute] int id)
        {
            var dc_to_delete = await dbContext.closingbilldetail.FirstOrDefaultAsync(d => d.id == id);
            if (dc_to_delete == null)
            {
                return NotFound();
            }
            dbContext.closingbilldetail.Remove(dc_to_delete);
            await dbContext.SaveChangesAsync();
            return Ok(dc_to_delete);
        }


        [HttpDelete]
        [Route("bulk_delete")]
        public async Task<IActionResult> Delete_bill([FromBody] List<int> id_list)
        {
            foreach (var id in id_list)
            {
                var dc_to_delete = await dbContext.closingbilldetail.FirstOrDefaultAsync(d => d.id == id);
                if (dc_to_delete == null)
                {
                    return NotFound();
                }
                dbContext.closingbilldetail.Remove(dc_to_delete);
                await dbContext.SaveChangesAsync();
            }
            return Ok("bulk delete complete");
        }
    }
}
