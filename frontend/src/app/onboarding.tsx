import { useState } from 'react';
import { Button, StyleSheet, Text, TextInput, View } from 'react-native';
import { useRouter } from 'expo-router';

import { createAndStoreUser } from '@/services/userService';

export default function OnboardingRoute() {
  const router = useRouter();
  const [username, setUsername] = useState('');
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  async function continueWithUsername() {
    const trimmedUsername = username.trim();

    if (!trimmedUsername) {
      setErrorMessage('Please enter a username.');
      return;
    }

    setErrorMessage(null);
    setIsSubmitting(true);

    try {
      await createAndStoreUser(trimmedUsername);
      router.replace('/');
    } catch (error) {
      setErrorMessage(
        error instanceof Error
          ? error.message
          : 'Unable to continue right now. Please try again.');
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <View style={styles.container}>
      <Text>Choose a username</Text>
      <TextInput
        value={username}
        onChangeText={setUsername}
        autoCapitalize="none"
        autoCorrect={false}
        editable={!isSubmitting}
        placeholder="Username"
        style={styles.input}
      />
      {errorMessage && <Text style={styles.error}>{errorMessage}</Text>}
      <Button
        title={isSubmitting ? 'Saving...' : 'Continue'}
        onPress={continueWithUsername}
        disabled={isSubmitting}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    padding: 24,
    gap: 12,
  },
  input: {
    borderWidth: 1,
    borderColor: '#999',
    borderRadius: 4,
    padding: 12,
  },
  error: {
    color: '#b00020',
  },
});
