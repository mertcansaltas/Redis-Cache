using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCaching.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		readonly IMemoryCache memoryCache;

		public ValuesController(IMemoryCache memoryCache)
		{
			this.memoryCache = memoryCache;
		}
		[HttpGet("SetName")]
		public void SetName(string name)
		{
			memoryCache.Set("name",name);
			
		}
		[HttpGet("GetName")]
		public string GetName()
		{
			if (memoryCache.TryGetValue<string>("name",out string name))
			{
				return name.Substring(0, name.Length-1);
			}
			return "";
		}
		[HttpGet("SetTime")]
		public void SetTime()
		{
			memoryCache.Set("Date",DateTime.Now, options: new()
			{
				AbsoluteExpiration= DateTime.Now.AddSeconds(30),	
				SlidingExpiration= TimeSpan.FromSeconds(5)
			});
		}
		[HttpGet("GetTime")]
		public DateTime GetTime()
		{
			return memoryCache.Get<DateTime>("Date");
		}
		[HttpGet("SetNameAndSurname")]
		public IActionResult SetNameAndSurname(string name, string surname)
		{
			memoryCache.Set("name",name, options: new()
			{
				AbsoluteExpiration=DateTime.Now.AddSeconds(30),
				SlidingExpiration=TimeSpan.FromSeconds(10)	
			});
			memoryCache.Set("surname",surname, options: new()
			{
				AbsoluteExpiration = DateTime.Now.AddSeconds(30),	
				SlidingExpiration = TimeSpan.FromSeconds(10)
			});
			return Ok();
		}
		[HttpGet("GetNameAndSurname")]
		public IActionResult GetNameAndSurname()
		{
			var name=memoryCache.Get("name");
			var surname=memoryCache.Get("surname");
			return Ok(new {name, surname});
		}
	}
}
