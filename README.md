# WakeOnLanPortal
Webbased portal using WakeOnLan library

This project is based on the Aquila Technology WakeOnLan library
Homepage: http://aquilawol.sourceforge.net/
Download: http://sourceforge.net/projects/aquilawol/files/Library/

By default web.config is configured to allow any user the access the portal.

Please create your own 'machines.xml' file bases on the sample.
(or use the AquilaWOL application to create one)
Download: http://sourceforge.net/projects/aquilawol/files/WakeOnLAN%202.x/

There's also an example on how to use the LDAP (Active directory) authentication for this projecty.
When you running the project in Visual Stusio, you can test the LDAP authentication.
On IIS it's possible to set the authentication on the Virtual Path level (Windows Authentication) and the LDAP authentication will automaticly use those credentials.
