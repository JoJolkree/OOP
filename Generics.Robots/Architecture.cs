using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Generics.Robots
{
    public interface IRobotAi<out T>
    {
        T GetCommand();
    }

    public class ShooterAI : IRobotAi<ShooterCommand>
    {
        int counter = 1;

        public ShooterCommand GetCommand()
        {
            return ShooterCommand.ForCounter(counter++);
        }
    }

    public class BuilderAI : IRobotAi<BuilderCommand>
    {
        int counter = 1;
        public BuilderCommand GetCommand()
        {
            return BuilderCommand.ForCounter(counter++);
        }
    }

    public interface IDevice<in T>
    {
        string ExecuteCommand(T command);
    }

    public class Mover<T> : IDevice<T>
        where T: IMoveCommand
    {
        public string ExecuteCommand(T command)
        {
            if (command == null)
                throw new ArgumentException();
            return $"MOV {command.Destination.X}, {command.Destination.Y}";
        }
    }

    public class Mover : Mover<IMoveCommand>
    {
    }

    public class Robot<T>
    {
        IRobotAi<T> ai;
        IDevice<T> device;

        public Robot(IRobotAi<T> ai, IDevice<T> executor)
        {
            this.ai = ai;
            this.device = executor;
        }

        public IEnumerable<string> Start(int steps)
        {
             for (int i=0;i<steps;i++)
            {
                var command = ai.GetCommand();
                if (command == null)
                    break;
                yield return device.ExecuteCommand(command);
            }

        }
    }

    public class Robot
    {
        public static Robot<IMoveCommand> Create(IRobotAi<IMoveCommand> ai, IDevice<IMoveCommand> executor)
        {
            return new Robot<IMoveCommand>(ai, executor);
        }
    }
}
