# Colony.Engine

Game-oriented module layout:

```text
Colony.Engine
├─ Composition
│  ├─ EngineServiceCollectionExtensions.cs
│  └─ ColonySimulationFactory.cs
├─ Simulation
│  ├─ Runtime
│  │  ├─ ColonySimulation.cs
│  │  ├─ SimulationEngine.cs
│  │  ├─ SimulationClock.cs
│  │  ├─ SimulationContext.cs
│  │  ├─ SimulationState.cs
│  │  └─ SimulationSpeed.cs
│  ├─ Time
│  │  └─ GameTime.cs
│  ├─ Systems
│  └─ SimulationSettings.cs
├─ Model
│  ├─ World
│  ├─ Population
│  └─ SimulationData.cs
├─ Generation
│  ├─ WorldFactory.cs
│  ├─ PopulationSeeder.cs
│  └─ TerrainGenerator.cs
└─ Contracts
   └─ ColonistView.cs
```

Responsibility split:
- `Composition`: DI registration and simulation composition root.
- `Simulation`: tick loop runtime, time flow, and ordered simulation systems.
- `Model`: mutable world/entity state used by the simulation.
- `Generation`: deterministic world/population setup for a new simulation instance.
- `Contracts`: renderer-facing read models.