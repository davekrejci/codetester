const path = require('path')

module.exports = {
  configureWebpack: {
    devtool: 'source-map'
},
  chainWebpack: config => {
    const apiClient = process.env.VUE_APP_API_CLIENT // mock or server
    config.resolve.alias.set(
      'api-client',
      path.resolve(__dirname, `src/api/${apiClient}`)
    )
    config.module
      .rule('raw')
      .test(/\.txt$/)
      .use('raw-loader')
      .loader('raw-loader')
      .end()
  },
  "transpileDependencies": [
    "vuetify"
  ],
}