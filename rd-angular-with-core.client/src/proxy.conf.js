const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
      "/UserAccount",
      "/RDData",
      "/users",
      "/LogData"
    ],
    target: "http://172.29.29.108:2025",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
