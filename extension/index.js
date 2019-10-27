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

function checkIcon(url) {
    if (url.includes('https://www.mobafire.com/league-of-legends/build')) {
        setIcon(true);
    } else {
        setIcon(false);
    }
}

chrome.tabs.onActivated.addListener((infos) => {
    chrome.tabs.get(infos.tabId, (tab) => checkIcon(tab.url));
});

chrome.webNavigation.onCompleted.addListener((infos) => {
    checkIcon(infos.url);
});

chrome.runtime.onInstalled.addListener(function() {
    console.log('Welcome to AutoRunes');
});

chrome.browserAction.onClicked.addListener(() => {
    sendMessage({ text: "Hello, my_application" });
});
