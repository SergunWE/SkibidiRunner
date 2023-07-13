mergeInto(LibraryManager.library, {
  getPlayerName: function () {
    try {
      var returnStr = player.getName();
      var bufferSize = lengthBytesUTF8(returnStr) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(returnStr, buffer, bufferSize);
      return buffer;
    } catch (err) {
      console.error(err);
      return null;
    }
  },

  getPlayerPhotoURL: function () {
    try {
      var returnStr = player.getPhoto("medium");
      var bufferSize = lengthBytesUTF8(returnStr) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(returnStr, buffer, bufferSize);
      return buffer;
    } catch (err) {
      console.error(err);
      return null;
    }
  },

  requestReviewGame: function () {
    ysdk.feedback.canReview().then(({ value, reason }) => {
      if (value) {
        ysdk.feedback.requestReview().then(({ feedbackSent }) => {
          console.log(feedbackSent);
        });
      } else {
        console.error(reason);
      }
    });
  },

  getReviewStatus: function () {
    try {
      ysdk.feedback.canReview().then(({ value, reason }) => {
        if (value) {
          return 0;
        } else {
          switch (reason) {
            case "NO_AUTH":
              return 1;
            case "GAME_RATED":
              return 2;
            case "REVIEW_ALREADY_REQUESTED":
              return 3;
            case "REVIEW_WAS_REQUESTED":
              return 4;
            case "UNKNOWN":
              return 1;
            default:
              return -1;
          }
        }
      });
    } catch (err) {
      console.error(err);
      return -1;
    }
  },

  savePlayerData: function (data) {
    try {
      var dateString = UTF8ToString(data);
      console.log(String(dateString));
      var myobj = JSON.parse(dateString);
      player.setData(myobj);
    } catch (err) {
      console.error(err);
    }
  },

  loadPlayerData: function () {
    try {
      player.getData().then((_date) => {
        var myJSON = JSON.stringify(_date);
        var bufferSize = lengthBytesUTF8(myJSON) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(myJSON, buffer, bufferSize);
        return buffer;
      });
    } catch (err) {
      console.error(err);
    }
  },

  setToLeaderboard: function (value) {
    try {
      ysdk.getLeaderboards().then((lb) => {
        lb.setLeaderboardScore("gameScore", value);
      });
    } catch (err) {
      console.error(err);
    }
  },

  getLang: function () {
      var lang = ysdk.environment.i18n.lang;
      var bufferSize = lengthBytesUTF8(lang) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(lang, buffer, bufferSize);
      return buffer;
  },
});
