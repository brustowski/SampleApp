const Agent = require('agentkeepalive');

// Keep-alive agent
const agent = new Agent({
    maxSockets: 100,
    keepAlive: true,
    maxFreeSockets: 10,
    keepAliveMsecs: 1000,
    timeout: 6000,
    keepAliveTimeout: 9000
});

// Windows authentication resolve
function onProxyRes(proxyRes) {
    let key = 'www-authenticate';
    proxyRes.headers[key] = proxyRes.headers[key] &&
        proxyRes.headers[key].split(',');
}

// Redirect settings
const proxy = {
    target: 'http://zs8-cbdev-web.luxoft.com/fp_dev/',
    secure: false,
    agent: agent,
    onProxyRes: onProxyRes,
};

// proxy paths
module.exports = {
    "/api/**": proxy,
    "/mvc/**": proxy,
    "/auth/**": proxy
};


