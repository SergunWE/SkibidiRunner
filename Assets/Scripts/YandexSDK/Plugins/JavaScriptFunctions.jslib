mergeInto(LibraryManager.library, {
  getPlayerName: function () {
    try {
      var returnStr = player.getName();
      var bufferSize = lengthBytesUTF8(returnStr) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(returnStr, buffer, bufferSize);
      return buffer;
    } catch (err) {
      console.err(err);
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
      console.err(err);
      return null;
    }
  },

  canReviewGame: function () {
    try {
      ysdk.feedback.canReview().then(({ value, reason }) => {
        if (value) {
          return true;
        } else {
          console.log(reason);
          return false;
        }
      });
    } catch (err) {
      console.err(err);
      return false;
    }
  },

  requestReviewGame: function () {
    ysdk.feedback.canReview().then(({ value, reason }) => {
      if (value) {
        ysdk.feedback.requestReview().then(({ feedbackSent }) => {
          console.log(feedbackSent);
          return feedbackSent;
        });
      } else {
        console.log(reason);
        return false;
      }
    });
  },

  gameRated: function () {
    try {
      ysdk.feedback.canReview().then(({ value, reason }) => {
        if (!value && reason == "GAME_RATED") {
          return true;
        } else {
          console.log(reason);
          return false;
        }
      });
    } catch (err) {
      console.err(err);
      return false;
    }
  },

  savePlayerData: function (data) {
    
  },

  loadPlayerData: function () {
    
  },
});
