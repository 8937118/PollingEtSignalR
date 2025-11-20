using labo.signalr.api.Data;
using labo.signalr.api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace labo.signalr.api.Hubs
{
    public class TaksHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public TaksHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            base.OnConnectedAsync();
            // TODO: Ajouter votre logique
            await Clients.All.SendAsync("TaskList", await _context.UselessTasks.ToListAsync());
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            base.OnDisconnectedAsync(exception);
            // TODO: Ajouter votre logique
        }


        public async Task GetAll()
        {
            await Clients.All.SendAsync("TaskList", await _context.UselessTasks.ToListAsync());
        }


        public async Task Add(string taskText)
        {
            UselessTask uselessTask = new UselessTask()
            {
                Completed = false,
                Text = taskText
            };
            _context.UselessTasks.Add(uselessTask);
            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("TaskList", await _context.UselessTasks.ToListAsync());
        }

        public async Task Complete(int id)
        {
            UselessTask? task = await _context.FindAsync<UselessTask>(id);
            if (task != null)
            {
                task.Completed = true;
                await _context.SaveChangesAsync();
          
            }

            await Clients.All.SendAsync("TaskList", await _context.UselessTasks.ToListAsync());

        }
    }
}
