const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
      "/UserAccount",
      "/RDData",
      "/users",
      "/LogData"
    ],
    target: "https://localhost:7277",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
