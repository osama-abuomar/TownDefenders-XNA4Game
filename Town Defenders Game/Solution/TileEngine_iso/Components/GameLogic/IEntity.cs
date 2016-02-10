namespace Engine.Components.GameLogic
{
    public interface IEntity
    {
        int Health { get; set; }
        int Defense { get; set; }
        int Attack { get; set; }

        int MaxHealth { get; set; }
        int MaxDefense { get; set; }
        int MaxAttack { get; set; }

        // general unit description
        string UnitDiscription { get;  }
        string PlayerDiscription { get;  }
        string HealthDiscription { get; }
        string DefenseDiscription { get;  }
        string AttackDiscription { get;  }
    
    }
}
