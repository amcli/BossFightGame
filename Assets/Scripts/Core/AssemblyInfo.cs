using System.Runtime.CompilerServices;

// Bootstrap is the only assembly allowed to mutate the Services facade.
// Gameplay code reads via the public getters; only Bootstrap initializes.
[assembly: InternalsVisibleTo("Game.Bootstrap")]
