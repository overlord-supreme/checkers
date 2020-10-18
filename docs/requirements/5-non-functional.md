
# 5. Non-Functional Requirements

## 5.1 Network Performance
### R5.1.1 Proton Reliability
Due to our networking choice using a dedicated server, we believe that any network lag in the game should be fairly minimal, as the only networked interaction will occur when passing the control back and forth between the players. Also, referring back to the dedicated server, if lag does occur, it should only affect the lagging player, creating an immediately more stable experience than peer-to-peer networking. Regardless, To ensure game quality, we will be playtesting the game before it's released multiple times, and we'll survey the playtesters to guarantee a more than acceptable quality for the game. **Priority 1**

### R5.1.2 Proton Scalability
We want our game to have a healthy option with its scalability, and with our current project's free subscription, the game will be able to support 20 concurrent players out of the box. However, thankfully, due to our networking API's flexibility, the player limit can be scaled up to 2000 simultaneous players with just the click of a button. While minor, we believe this scalability provides us a comfortable position where we can practically guarantee that any small userbase we might gather will be satisfied with their quick and reliable online gameplay. **Priority 1**
<br/>
<br/>
<br/>
<br/>

## 5.2  Operating System Requirements
### R5.2.1 Supported Platforms
The game is expected to support Windows 7, 8.1, and 10 along with Linux. Other Operating Systems will not be tested and built for, so they are not expected to work with Overlord Supreme. **Priority 1**
<br/>
<br/>
<br/>
<br/>

## 5.3 Availability
### R5.3.1 Client Download
The Overlord Supreme client application will be downloadable from our *Github* releases. **Priority 1**
<br/>
<br/>
<br/>
<br/>

## 5.4 Security
### R5.4.1 User Privacy
Overlord Supreme will collect no data about the user running the game, as we believe in allowing our players to use our service without getting something back from them. In furthering our considerations about privacy, the networking tool Proton itself collects no data about the user aside from their IP and their unique Proton authentication code. **Priority 1** 

### R5.4.2 Network Authentication / Encryption
To connect to another player using Proton, all we ask of the player is their authentication key (which should be automatically generated on game boot). From there, that authentication gets passed along to Proton as encrypted data, keeping the details of the connection safe from any potential outside attackers. **Priority 2** 
<br/>
<br/>
<br/>
<br/>

## 5.5 Usability
### R5.5.1 Playtesting
At each stable release of Overlord Supreme, the team will attempt to run a minor playtesting session with between 5 to 10 users. Our goal with these playtesting sessions is to pull players that are mostly unfamiliar with the project to get their unbiased opinion about the game's current functionality and enjoyability. To make their reports more actionable, we will also be providing them with a form to fill out so that we can better analyze any found shortcomings of the product.
<br/>
<br/>
<br/>
<br/>

## 5.6 Maintainability
### R5.6.1 Potential Future of Updates
As time goes on, our game will likely begin to have networking issues as the age of the version of Photon we're using increases. However, due to the excellent documentation on the API, future updates to resolve these issues should be incredibly simple, as they seem to provide a per-release update guide, making it very easy to refactor the code and push out the update in lightning speed. **Priority 2**
<br/>
<br/>
<br/>
<br/>

