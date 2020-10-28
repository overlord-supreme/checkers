# 5 Data Model and Storage

### Data Model

The figure below depicts the UML for the Profile data model.

```mermaid
classDiagram
    class Profile {
          -Name: String
          +Profile(Name: String)
          +GetName() String
          +SetName(name: String)
    }
```

The figure below depicts the UML for the data storage.

```mermaid
classDiagram
    IProfileStore <|-- JSONProfileStore: implements
    IProfileStore *-- DataModel: composition

    class DataModel {
        -ProfileStore: IProfileStore
        +SetName(name: String)
        +GetName() name
    }

    class IProfileStore {
        +GetProfile() Profile
        +SetProfile(Profile profile)
        +Save()
        +Load()
    }
    <<interface>> IProfileStore

    class JSONProfileStore {
        -profile: Profile
        +JSONProfileStore(file: String)
        +JSONProfileStore()
        +GetProfile() Profile
        +SetProfile(Profile profile)
        +Save()
        +Load()   
    }
```


### 5.1 DataModel

`DataModel` provides a facade for the data storage library.

#### 5.1.1 Attributes

| Name         | Type          | Description                          |
|--------------|---------------|--------------------------------------|
| ProfileStore | IProfileStore | Used to access the player's profile. |

#### 5.1.2 Methods

| SetName(name: String) |                                                    |
|-----------------------|----------------------------------------------------|
| Input                 | The player's new username.                         |
| Output                | Void                                               |
| Description           | Modifies the user's current username and saves it. |

| GetName(): String |                                          |
|-------------------|------------------------------------------|
| Input             | Void                                     |
| Output            | The player's current username.           |
| Description       | Retrieves the player's current username. |


### 5.2 IProfileStore

`IProfileStore` is an interface that provides resources for accessing the
user's profile.

#### 5.2.1 Methods

| GetProfile(): Profile |                                                     |
|-----------------------|-----------------------------------------------------|
| Input                 | Void                                                |
| Output                | The player's current profile.                       |
| Description           | Retrieves the player's current profile from memory. |

| SetProfile(profile: Profile) |                                                  |
|------------------------------|--------------------------------------------------|
| Input                        | The player's new profile.                        |
| Output                       | Void                                             |
| Description                  | Modifies the player's current profile in memory. |

| Save()      |                                                            |
|-------------|------------------------------------------------------------|
| Input       | Void                                                       |
| Output      | Void                                                       |
| Description | Saves the state of the player's current profile in memory. |

| Load()      |                                                              |
|-------------|--------------------------------------------------------------|
| Input       | Void                                                         |
| Output      | Void                                                         |
| Description | Loads the state of the player's current profile into memory. |

### 5.3 JSONProfileStore

`JSONProfileStore` is an implementation of `IProfileStore` that stores the
player's profile in a JSON file.

#### 5.3.1 Attributes

| Name    | Type    | Description                                         |
|---------|---------|-----------------------------------------------------|
| Profile | Profile | An in-memory state of the player's current profile. |

#### 5.3.2 Methods

| GetProfile(): Profile |                                                     |
|-----------------------|-----------------------------------------------------|
| Input                 | Void                                                |
| Output                | The player's current profile.                       |
| Description           | Retrieves the player's current profile from memory. |

| SetProfile(profile: Profile) |                                                  |
|------------------------------|--------------------------------------------------|
| Input                        | The player's new profile.                        |
| Output                       | Void                                             |
| Description                  | Modifies the player's current profile in memory. |

| Save()      |                                                                 |
|-------------|-----------------------------------------------------------------|
| Input       | Void                                                            |
| Output      | Void                                                            |
| Description | Saves the state of the player's current profile to a JSON file. |

| Load()      |                                                                               |
|-------------|-------------------------------------------------------------------------------|
| Input       | Void                                                                          |
| Output      | Void                                                                          |
| Description | Loads the state of the player's current profile into memory from a JSON file. |

