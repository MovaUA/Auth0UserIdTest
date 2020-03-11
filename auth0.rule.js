function(user, context, callback) {
    const userIdMetadata = "debriefit-user-id";

    if (!(user.app_metadata && user.app_metadata[userIdMetadata])) {
        callback(null, user, context);
        return;
    }



    const userId = user.app_metadata[userIdMetadata];
    const userIdClaim = "https://debriefit.com/user-id";

    if (context.idToken) {
        context.idToken[userIdClaim] = userId;
    }

    if (context.accessToken) {
        context.accessToken[userIdClaim] = userId;
    }

    callback(null, user, context);
}


function(user, context, callback) {
    if (user.app_metadata) {
        const userId = user.app_metadata["debriefit-user-id"];

        if (userId) {
            const userIdClaim = "https://debriefit.com/user-id";

            if (context.idToken) {
                context.idToken[userIdClaim] = userId;
            }

            if (context.accessToken) {
                context.accessToken[userIdClaim] = userId;
            }
        }
    }

    callback(null, user, context);
}

function(user, context, callback) {
    if (!user.app_metadata) {
        callback(null, user, context);
        return;
    }

    const userId = user.app_metadata["debriefit-user-id"];

    if (!userId) {
        callback(null, user, context);
        return;
    }

    const userIdClaim = "https://debriefit.com/user-id";

    if (context.idToken) {
        context.idToken[userIdClaim] = userId;
    }

    if (context.accessToken) {
        context.accessToken[userIdClaim] = userId;
    }

    callback(null, user, context);
}


function(user, context, callback) {
    const userId = user.app_metadata && user.app_metadata["debriefit-user-id"];

    if (!userId) {
        callback(null, user, context);
        return;
    }

    const userIdClaim = "https://debriefit.com/user-id";

    if (context.idToken) {
        context.idToken[userIdClaim] = userId;
    }

    if (context.accessToken) {
        context.accessToken[userIdClaim] = userId;
    }

    callback(null, user, context);
}