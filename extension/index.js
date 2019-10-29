var port = null;
let currentStatus = false;

function setIcon(status) {
    currentStatus = status;
    if (status) {
        chrome.contextMenus.create({
            id: 'notInGame',
            title: "Not in a game fill",
            contexts: ['page', 'browser_action'],
        });
        chrome.browserAction.setIcon({ path: "assets/rune.png" });
    } else {
        chrome.contextMenus.removeAll();
        chrome.browserAction.setIcon({ path: "assets/rune_off.png" });
    }
}

function setUpPort() {
    port = chrome.runtime.connectNative('com.yooooomi.autorunes');
    port.onMessage.addListener(function(msg) {
        console.log("Received" + JSON.stringify(msg));
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

function notInGameParse(click, tab) {
    sendMessage({ text: 'url:' + tab.url, inGame: false });
}

const itemidToFunction = {
    'notInGame': notInGameParse,
};

chrome.browserAction.setIcon({ path: "assets/rune_off.png" });

chrome.contextMenus.onClicked.addListener((click, tab) => {
    itemidToFunction[click.menuItemId](click, tab);
});

chrome.tabs.onActivated.addListener((infos) => {
    chrome.tabs.get(infos.tabId, (tab) => checkIcon(tab.url));
});

chrome.webNavigation.onCompleted.addListener((infos) => {
    checkIcon(infos.url);
});

chrome.runtime.onInstalled.addListener(function() {
    console.log('Welcome to AutoRunes');
});

chrome.browserAction.onClicked.addListener((tab) => {
    //alert("Do not touch anything !");
    if (currentStatus === true) {
        sendMessage({ text: 'url:' + tab.url, inGame: true });
    }
});
