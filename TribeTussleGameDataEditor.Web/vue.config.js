const path = require('path');

module.exports = {
    devServer: {
        proxy: 'http://localhost:51051'
    },
    outputDir: path.resolve(__dirname, '../TribeTussleGameDataEditor.API/wwwroot')
}

process.env.VUE_APP_DEV_DATA_ROOT = module.exports.devServer.proxy;