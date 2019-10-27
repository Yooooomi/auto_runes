var port = null;

function setIcon(status) {
    if (status) {
        chrome.browserAction.setIcon({ path: "assets/rune.png" });
    } else {
        chrome.browserAction.setIcon({ path: "assets/rune_off.png" });
    }
}

function setUpPort() {
    port = chrome.runtime.connectNative('com.yooooomi.autorunes');
    port.onMessage.addListener(function(msg) {
        console.log("Received" + msg);
    });
    port.onDisconnect.addListener(function() {
        console.log("Disconnected");
        port = null;
    });
}

function sendMessage(msg) {
    if (port === null) {
        setUpPort();
    }
    port.postMessage(msg);
}

chrome.runtime.onInstalled.addListener(function() {
    console.log('Welcome to AutoRunes');
});

chrome.webNavigation.onCompleted.addListener(() => {
    console.log('initial');
    setIcon(false);
});

chrome.browserAction.onClicked.addListener(() => {
    sendMessage({ text: "Hello, my_application" });
});

chrome.webNavigation.onCompleted.addListener(() => {
    console.log('build');
    setIcon(true);
}, {
    url: [{urlContains: 'https://www.mobafire.com/league-of-legends/build'}],
});
