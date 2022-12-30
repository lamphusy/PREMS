//upper to 5 letters
function generatePassword(numberOfChar) {
    var password = "@";
    password += String.fromCharCode((Math.random() * 26) + 65);
    password += String.fromCharCode((Math.random() * 10) + 48);
    for (i = 0; i < numberOfChar - 3; i++) {
        if (Math.random() * 2 == 0) {
            password += String.fromCharCode((Math.random() * 26) + 65);
        }
        else {
            password += String.fromCharCode((Math.random() * 26) + 97);

        }
    }
    return password;
}