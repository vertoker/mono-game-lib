# SceneLib

Custom library for Monogame, which can manage scenes and objects in it

Game begins with class with IGameSetup interface. Using GameContext you can register project and regular scenes

Every Scene is a class with ISceneSetup interface. Using SceneContext you can register services with various options

For services exists interfaces? which calls in original Game and only in borders of Scene lifecycle

- IServiceSetup (SceneContext)

- IServiceInitializable
- IServiceUpdatable (GameTime)
- IServiceDrawable (GameTime)
- IServiceDisposable

- IContentLoad (ContentManager)
- IContentUnload (ContentManager)

I recommended create Services with parameterless constructors

Every Scene include inself DI Container (no constructors)

On that library working Bullet Hero (my game)